#!/bin/bash -xe

executable="publish/LegendsViewer"
app=".github/scripts/LegendsViewer.app"
libskiasharp="publish/libSkiaSharp.dylib"
bundle_id="com.kromtec.legendsviewer-next"

if [ "$CI" = "true" ]; then
    # get the signing certificate (this is the Developer ID: Application: Your Name, exported to a p12 file, then converted to base64, e.g.: cat ~/Desktop/certificate.p12 | base64 | pbcopy)
    echo $MACOS_CERTIFICATE_P12 | base64 --decode > certificate.p12

    # create a keychain
    security create-keychain -p secret build.keychain
    security default-keychain -s build.keychain
    security unlock-keychain -p secret build.keychain

    # import the key
    security import certificate.p12 -k build.keychain -P "${MACOS_CERTIFICATE_PASSWORD}" -T /usr/bin/codesign

    security set-key-partition-list -S apple-tool:,apple: -s -k secret build.keychain

    # check if all is good
    security find-identity -v
fi

# sign libskia
codesign -s "${MACOS_DEVELOPER_ID}" --timestamp --options runtime -f --deep "${libskiasharp}"

# sign the app
codesign -s "${MACOS_DEVELOPER_ID}" --timestamp --options runtime -f --deep "${executable}"

# copy into app skeleton
cp -r ./publish/ "${app}/Contents/MacOS"

codesign -s "${MACOS_DEVELOPER_ID}" --timestamp --options runtime -f --deep "${app}"

if ! command -v xcrun >/dev/null || ! xcrun --find notarytool >/dev/null; then
    echo "Notarytool is not present in the system. Notarization has failed."
    exit 1
fi


mkdir release

# this create-dmg script kinda sucks but the output is pretty ¯\_(ツ)_/¯
npm install --global create-dmg
create-dmg "${app}" ./release || true

dmg=$(ls ./release/*.dmg)

codesign -s "${MACOS_DEVELOPER_ID}" --timestamp --options runtime -f --deep "${dmg}"

# Submit the package for notarization
notarization_output=$(
    xcrun notarytool submit "${dmg}" \
        --apple-id "hello@koenschmeets.nl" \
        --password "${MACOS_APPSTORE_APP_PASSWORD}" \
        --team-id "8X77K9NDG3" \
        --wait 2>&1)

if [ $? -eq 0 ]; then
    echo "Notarization succeeded."
    echo $notarization_output
    xcrun stapler staple "${dmg}"
    exit 0
  else
    echo "Notarization failed. Error: $notarization_output"
    exit 1
  fi
fi

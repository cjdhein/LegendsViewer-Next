import createClient from "openapi-fetch";
import type { paths } from "./generated/api-schema";

const client = createClient<paths>({ baseUrl: "http://localhost:5054/" });

export default client;
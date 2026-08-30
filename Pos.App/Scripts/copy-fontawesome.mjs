import { copyFileSync, cpSync, mkdirSync } from "node:fs";

const cssTarget = "wwwroot/lib/fontawesome/css";
const webfontsTarget = "wwwroot/lib/fontawesome/webfonts";

mkdirSync(cssTarget, { recursive: true });
mkdirSync(webfontsTarget, { recursive: true });

copyFileSync(
  "node_modules/@fortawesome/fontawesome-free/css/all.min.css",
  `${cssTarget}/all.min.css`,
);

cpSync("node_modules/@fortawesome/fontawesome-free/webfonts", webfontsTarget, { recursive: true });

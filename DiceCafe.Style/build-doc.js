const showdown = require("showdown");
const fs = require("fs").promises;
const converter = new showdown.Converter();

async function main() {
  const content = await fs.readFile("../dicescript_reference.md");
  const template = await fs.readFile("./template.html");
  const md = converter.makeHtml(content.toString());
  const fulldoc = template
    .toString()
    .replace("*content*", md)
    .replace("*title*", "dicescript reference");
  await fs.writeFile(
    "../DiceCafe.WebApp/wwwroot/dicescript_reference.html",
    fulldoc
  );
}

main();

module.exports = {
  purge: {
    enabled: true,
    content: [
      "./src/**/*.tsx",
      "./src/**/*.ts",
      "../DiceCafe.WebApp/Views/**/*.cshtml",
      "../DiceCafe.WebApp/Pages/**/*.cshtml",
    ],
  },
  theme: {
    extend: {},
  },
  variants: {},
  plugins: [],
};

module.exports = {
  purge: {
    enabled: true,
    content: [
      "../DiceCafe.Client/src/**/*.tsx",
      "../DiceCafe.Client/src/**/*.ts",
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

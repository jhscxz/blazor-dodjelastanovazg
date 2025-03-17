import colors from 'tailwindcss/colors'

export default {
    content: [
        "./Pages/**/*.razor",
        "./Components/**/*.razor",
        "./Shared/**/*.razor",
        "./wwwroot/index.html"
    ],
    theme: {
        extend: {
            colors: {
                blue: colors.blue,
            },
        },
    },
    plugins: [],
}

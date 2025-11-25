module.exports = {
    content: [
        './**/*.razor',
        './wwwroot/index.html',
        './wwwroot/**/*.html',
        './**/*.cshtml'
    ],
    theme: {
        extend: {
            colors: {
                blue: require('tailwindcss/colors').blue,
            },
            boxShadow: {
                'md': '0 4px 6px -1px rgba(0, 0, 0, 0.1), 0 2px 4px -2px rgba(0, 0, 0, 0.1)',
                'xl': '0 20px 25px -5px rgba(0, 0, 0, 0.1), 0 8px 10px -6px rgba(0, 0, 0, 0.1)',
            },
        },
    },
    plugins: [],
    safelist: [
        'border-gray-300',
        'hover:border-gray-400',
        'shadow-md',
        'hover:shadow-xl'
    ],

};

import type { Config } from "tailwindcss";
import plugin from "tailwindcss";

const config: Config = {
    content: [
        "./pages/**/*.{js,ts,jsx,tsx,mdx}",
        "./components/**/*.{js,ts,jsx,tsx,mdx}",
        "./app/**/*.{js,ts,jsx,tsx,mdx}",
    ],
    theme: {
        extend: {
            backgroundImage: {
                "gradient-radial": "radial-gradient(var(--tw-gradient-stops))",
                "gradient-conic":
                    "conic-gradient(from 180deg at 50% 50%, var(--tw-gradient-stops))",
            },
            colors: {
                background: {
                    secondary: "#181818",
                    "inline-button": "#262727",
                    "text-fields": "#121313",
                },
                text: {
                    placeholder: "#9F9F9F",
                },
                accent: {
                    orange: "#FF7700",
                },
            },
            margin: {
                "page-part": ".5em",
            },
        },
    },
    plugins: [],
};
export default config;

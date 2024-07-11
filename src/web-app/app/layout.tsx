import type { Metadata } from "next";
import "./globals.css";
import Navbar from "./components/nav/Navbar";

export const metadata: Metadata = {
    title: "Platform",
    description: "Social media platform",
};

export default function RootLayout({
    children,
}: Readonly<{
    children: React.ReactNode;
}>) {
    return (
        <html lang="en">
            <body>
                <Navbar />
                {children}
            </body>
        </html>
    );
}

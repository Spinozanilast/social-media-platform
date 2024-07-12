import type { Metadata } from "next";
import Logo from "./components/nav/Logo";
import "./globals.css";

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
                <div className="logo-container mt-page-part flex justify-center">
                    <Logo animationType="backward" />
                </div>
                {children}
            </body>
        </html>
    );
}

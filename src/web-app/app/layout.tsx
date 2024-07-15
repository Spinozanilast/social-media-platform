import type { Metadata } from "next";
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
            <body className="min-h-[100vh]">{children}</body>
        </html>
    );
}

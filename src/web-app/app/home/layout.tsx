import type { Metadata } from "next";
import "@app/globals.css";
import Navbar from "./../components/nav/Navbar";

export const metadata: Metadata = {
    title: "Platform",
    description: "Social media platform",
};

export default function Layout({
    children,
}: Readonly<{
    children: React.ReactNode;
}>) {
    return (
        <section>
            <div className="logo-container mt-page-part flex justify-center">
                <Navbar />
            </div>
            {children}
        </section>
    );
}

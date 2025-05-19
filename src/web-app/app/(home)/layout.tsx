import type { Metadata } from 'next';
import '@app/globals.css';
import Navbar from '@components/layout/navbar/Navbar';

export const metadata: Metadata = {
    title: 'Platform',
    description: 'Social media platform',
};

export default function Layout({
    children,
}: Readonly<{
    children: React.ReactNode;
}>) {
    return (
        <div>
            <div className="logo-container flex justify-center">
                <Navbar />
            </div>
            {children}
        </div>
    );
}

import type { Metadata } from 'next';
import Navbar from '~/components/layout/navbar/navbar';

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
            <div className="m-2 logo-container flex justify-center">
                <Navbar />
            </div>
            {children}
        </div>
    );
}

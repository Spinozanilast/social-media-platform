import Navbar from '../components/nav/Navbar';
import Copyright from '@themes/mui-components/Copyright';

export default function Layout({
    children,
}: Readonly<{
    children: React.ReactNode;
}>) {
    return (
        <div className="selection:bg-cyan-500 flex items-center flex-col min-h-[inherit]">
            <div className="logo-container mt-page-part flex justify-center m-2">
                <Navbar />
            </div>
            {children}
            <Copyright sx={{ mt: 6, mb: 4 }} />
        </div>
    );
}

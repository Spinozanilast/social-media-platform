import Navbar from '@/app/components/common/Navbar';
import Copyright from '@components/common/Copyright';

export default function Layout({
    children,
}: Readonly<{
    children: React.ReactNode;
}>) {
    return (
        <div className="selection:bg-cyan-500 flex items-center flex-col">
            <div className="logo-container mt-page-part flex justify-center m-2">
                <Navbar withCurrentUser={true} />
            </div>
            {children}
            <Copyright />
        </div>
    );
}

import Navbar from '~/components/layout/navbar/navbar';
import Copyright from '~/components/layout/copyright';

export default function Layout({
    children,
}: Readonly<{
    children: React.ReactNode;
}>) {
    return (
        <div className="selection:bg-cyan-500 flex min-h-[100vh] items-center flex-col">
            <div className="logo-container mt-page-part flex justify-center w-full">
                <Navbar />
            </div>
            <div className="my-auto">{children}</div>
            <Copyright mt={2} />
        </div>
    );
}

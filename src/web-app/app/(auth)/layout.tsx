import Copyright from '@themes/mui-components/Copyright';
import Logo from '@app/components/nav/Logo';

export default function Layout({
    children,
}: Readonly<{
    children: React.ReactNode;
}>) {
    return (
        <section className="selection:bg-cyan-500 flex flex-col min-h-[inherit]">
            <div className="flex my-auto flex-col items-center justify-center">
                <div className="logo-container mt-page-part flex justify-center">
                    <Logo animationType="backward" className="text-center" />
                </div>
                <div className="">{children}</div>
                <Copyright sx={{ mt: 6, mb: 4 }} />
            </div>
        </section>
    );
}

import Copyright from '@components/common/Copyright';
import Logo from '@/app/components/common/Logo';

export default function Layout({
    children,
}: Readonly<{
    children: React.ReactNode;
}>) {
    return (
        <section className="selection:bg-cyan-500 flex flex-col justify-between min-h-[inherit]">
            <div className="logo-container mt-page-part flex justify-center">
                <Logo animationType="backward" className="text-center" />
            </div>
            <div className="flex flex-col items-center justify-center">
                <div>{children}</div>
            </div>
            <Copyright mt={1} />
        </section>
    );
}

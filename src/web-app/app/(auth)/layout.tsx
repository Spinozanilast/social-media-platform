import Copyright from "../themes/mui-components/Copyright";
import Logo from "@app/components/nav/Logo";

export default function Layout({
    children,
}: Readonly<{
    children: React.ReactNode;
}>) {
    return (
        <section className=" selection:bg-cyan-500">
            <div className="logo-container mt-page-part flex justify-center">
                <Logo animationType="backward" className="text-center" />
            </div>
            {children}
            <Copyright sx={{ mt: 6, mb: 4 }} />
        </section>
    );
}

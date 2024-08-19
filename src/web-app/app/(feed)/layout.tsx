import SearchBar from "../components/nav/SearchBar";
import Copyright from "../themes/mui-components/Copyright";
import Logo from "@app/components/nav/Logo";

export default function Layout({
    children,
}: Readonly<{
    children: React.ReactNode;
}>) {
    return (
        <section className="selection:bg-cyan-500 flex items-center flex-col min-h-[inherit]">
            <SearchBar placeholderText="Search..." className="mt-page-part" />
            <div className="">{children}</div>
            <Copyright sx={{ mt: 6, mb: 4 }} />
        </section>
    );
}

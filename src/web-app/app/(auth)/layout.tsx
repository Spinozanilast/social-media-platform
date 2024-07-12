import Copyright from "../themes/mui-components/Copyright";

export default function RootLayout({
    children,
}: Readonly<{
    children: React.ReactNode;
}>) {
    return (
        <html lang="en">
            <body>
                {children}
                <Copyright sx={{ mt: 8, mb: 4 }} />{" "}
            </body>
        </html>
    );
}

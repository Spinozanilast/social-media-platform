import Copyright from '~/components/layout/copyright';
import Logo from '~/components/layout/logo';
import NavSwitchers from '../components/common/navigation-switchers';
import React from 'react';

export default function Layout({
    children,
}: Readonly<{
    children: React.ReactNode;
}>) {
    return (
        <section className="selection:bg-cyan-500 flex flex-col justify-between min-h-[inherit]">
            <div className="absolute ml-page-part mt-page-part">
                <NavSwitchers langBadgeOnLeft={false} />
            </div>
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

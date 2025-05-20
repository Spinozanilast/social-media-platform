import React from 'react';
import Logo from '~components/layout/logo';
import NavbarButtons from '~components/layout/navbar/navbar-buttons';
import NavSwitchers from '~components/common/navigation-switchers';

interface NavbarProps {
    withUser?: boolean;
    includeUrlCompleteItems?: boolean;
}

const Navbar: React.FC<NavbarProps> = () => {
    return (
        <div className="w-full flex mx-page-part flex-row justify-between h-8 items-center m-2">
            <div className="logo-container h-fit">
                <Logo animationType="forward" className="text-left" />
            </div>
            <div className="flex flex-row gap-2 items-center m-2">
                <NavbarButtons />
            </div>
            <NavSwitchers />
        </div>
    );
};

export default Navbar;

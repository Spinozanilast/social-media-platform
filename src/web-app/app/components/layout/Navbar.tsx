import CurrentUser from '@/app/components/layout/CurrentUser';
import React from 'react';
import Logo from './Logo';
import { ThemeSwitcher } from './ThemeSwitcher';
import NavbarButtons from './NavbarButtons';
import { PersonalUrlPagesItems } from '@/app/data/VisualCompleteItemData';
import LangSwitchSelect from '../special/LangSwitchSelect';
import NavSwitchers from '../common/NavSwitchers';

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
                <NavbarButtons items={PersonalUrlPagesItems} />
            </div>
            <NavSwitchers />
        </div>
    );
};

export default Navbar;

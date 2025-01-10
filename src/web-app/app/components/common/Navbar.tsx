import CurrentUser from '@components/CurrentUser';
import React from 'react';
import Logo from './Logo';
import { ThemeSwitcher } from './ThemeSwitcher';
import PagesNavbarButtons from './PagesNavbarButtons';
import { PersonalUrlPagesItems } from '@/app/data/VisualCompleteItemData';
import LangSwitchSelect from '../utils/LangSwitchSelect';
import NavSwitchers from './NavSwitchers';

interface NavbarProps {
    withCurrentUser?: boolean;
    includeUrlCompleteItems?: boolean;
}

const Navbar: React.FC<NavbarProps> = ({ withCurrentUser = false }) => {
    return (
        <div className="w-full flex mx-page-part flex-row justify-between h-8 items-center">
            <div className="logo-container h-fit">
                <Logo animationType="forward" className="text-left" />
            </div>
            <div className="flex flex-row gap-2 items-center m-2">
                {withCurrentUser && <CurrentUser />}
                <PagesNavbarButtons items={PersonalUrlPagesItems} />
            </div>
            <NavSwitchers />
        </div>
    );
};

export default Navbar;

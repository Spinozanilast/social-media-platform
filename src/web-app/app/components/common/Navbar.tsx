import CurrentUser from '@components/CurrentUser';
import React from 'react';
import Logo from './Logo';
import SearchBar from './SearchBar';
import { urlCompleteItems } from '@data/CompleteItemsData/UrlCompleteItemData';

interface NavbarProps {
    withCurrentUser?: boolean;
    includeUrlCompleteItems?: boolean;
}

const Navbar: React.FC<NavbarProps> = ({ withCurrentUser = false }) => {
    return (
        <div className="mx-auto flex flex-row justify-center h-8 items-center m-2">
            {withCurrentUser && <CurrentUser />}
            <div className="logo-container absolute mt-page-part ml-page-part h-fit">
                <Logo animationType="forward" className="text-left" />
            </div>
            <SearchBar
                placeholderText="Search For ..."
                urlCompletions={urlCompleteItems}
            />
        </div>
    );
};

export default Navbar;

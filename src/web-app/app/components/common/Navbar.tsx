import CurrentUser from '@components/CurrentUser';
import React from 'react';
import Logo from './Logo';
import SearchBar from './SearchBar';

interface NavbarProps {
    withCurrentUser?: boolean;
}

const Navbar: React.FC<NavbarProps> = ({ withCurrentUser = false }) => {
    return (
        <div className="mx-auto flex flex-row justify-center h-8 items-center m-2">
            <CurrentUser />
            <div className="logo-container absolute mt-page-part ml-page-part h-fit">
                <Logo animationType="forward" className="text-left" />
            </div>
            <SearchBar
                className="flex flex-row items-center"
                placeholderText="Search For ..."
            />
        </div>
    );
};

export default Navbar;

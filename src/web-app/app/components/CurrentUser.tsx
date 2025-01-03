'use client';
import Image from 'next/image';
import React from 'react';
import { useTranslations } from 'next-intl';
import Menu from '@mui/material/Menu';
import MenuItem from '@mui/material/MenuItem';
import Divider from '@mui/material/Divider';
import { theme } from '../themes/main-dark';
import { ThemeProvider } from '@emotion/react';
import { ListItemIcon } from '@mui/material';
import { FaUser } from 'react-icons/fa';
import { BsThreeDots } from 'react-icons/bs';
import { CiLogout } from 'react-icons/ci';
import { IconContext } from 'react-icons/lib';
import IdentityService from '../api/services/user';

const CurrentUserTranslationSection: string = 'CurrentUser';

const CurrentUser = () => {
    // const t = useTranslations(CurrentUserTranslationSection);

    const imageUrl: string = '/profile.svg';

    const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
    const open = Boolean(anchorEl);
    const handleClick = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorEl(event.currentTarget);
    };
    const handleClose = () => {
        setAnchorEl(null);
    };

    const handleLogout = async () => {
        console.log(await IdentityService.logOut());
        setAnchorEl(null);
    };

    return (
        <React.Fragment>
            <ThemeProvider theme={theme}>
                <div className="m-2 relative h-full min-w-[36px] min-h-[36px]">
                    <Image
                        onClick={handleClick}
                        className="rounded-md p-2 shadow-accent-orange shadow-inner hover:shadow-lg hover:border-l-2 hover:border-accent-orange hover:border-solid"
                        src={imageUrl}
                        alt="curret user profile image"
                        width={36}
                        height={36}
                    />
                </div>

                <Menu
                    anchorEl={anchorEl}
                    id="account-menu"
                    className="rounded-md"
                    open={open}
                    onClose={handleClose}
                    onClick={handleClose}
                    transformOrigin={{ horizontal: 'right', vertical: 'top' }}
                    anchorOrigin={{ horizontal: 'left', vertical: 'top' }}
                >
                    <IconContext.Provider
                        value={{ className: 'text-accent-orange' }}
                    >
                        <MenuItem onClick={handleClose}>
                            <ListItemIcon>
                                <FaUser />
                            </ListItemIcon>
                            Profile
                        </MenuItem>
                        <MenuItem onClick={handleClose}>My account</MenuItem>
                        <Divider />
                        <MenuItem onClick={handleClose}>
                            <ListItemIcon>
                                <BsThreeDots />
                            </ListItemIcon>
                            Settings
                        </MenuItem>
                        <MenuItem onClick={handleLogout}>
                            <ListItemIcon>
                                <CiLogout />
                            </ListItemIcon>
                            Logout
                        </MenuItem>
                    </IconContext.Provider>
                </Menu>
            </ThemeProvider>
        </React.Fragment>
    );
};

export default CurrentUser;

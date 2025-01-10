import { IconType } from 'react-icons/lib';
import CompleteItemData from './CompleteItemData';
import { TiMessages } from 'react-icons/ti';
import { FaBookOpen } from 'react-icons/fa6';
import { MdOutlineSpaceDashboard } from 'react-icons/md';

export type VisualCompleteItemData = CompleteItemData & {
    icon: IconType;
};

export const PersonalUrlPagesItems: VisualCompleteItemData[] = [
    {
        id: 1,
        name: 'messages',
        url: '/messages',
        icon: TiMessages,
    },
    {
        id: 2,
        name: 'stories',
        url: '/stories',
        icon: FaBookOpen,
    },
    {
        id: 3,
        name: 'board',
        url: '/board',
        icon: MdOutlineSpaceDashboard,
    },
];

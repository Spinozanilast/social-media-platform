import {
    LucideProps,
    MessageSquare,
    BookOpenText,
    LayoutDashboard,
} from 'lucide-react';

export default interface CompleteItemData {
    id: number;
    name: string;
    url: string;
}

export type VisualCompleteItemData = CompleteItemData & {
    icon: React.ForwardRefExoticComponent<
        Omit<LucideProps, 'ref'> & React.RefAttributes<SVGSVGElement>
    >;
};

export const PersonalUrlPagesItems: VisualCompleteItemData[] = [
    {
        id: 1,
        name: 'messages',
        url: '/messages',
        icon: MessageSquare,
    },
    {
        id: 2,
        name: 'stories',
        url: '/stories',
        icon: BookOpenText,
    },
    {
        id: 3,
        name: 'board',
        url: '/board',
        icon: LayoutDashboard,
    },
];

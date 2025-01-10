'use client';

import LangSwitchSelect from '../utils/LangSwitchSelect';
import { ThemeSwitcher } from './ThemeSwitcher';

export type NavSwitcherProps = {
    langBadgeOnLeft?: boolean;
};

export default function NavSwitcher({
    langBadgeOnLeft = true,
}: NavSwitcherProps) {
    return (
        <div className="flex flex-row gap-2 items-center">
            <LangSwitchSelect badgeOnLeft={langBadgeOnLeft} />
            <ThemeSwitcher />
        </div>
    );
}

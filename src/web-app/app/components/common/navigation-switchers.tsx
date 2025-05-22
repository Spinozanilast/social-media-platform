'use client';

import LangSwitchSelect from '~/components/common/lang-switch-select';
import { ThemeSwitcher } from '~components/layout/theme-switcher';

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

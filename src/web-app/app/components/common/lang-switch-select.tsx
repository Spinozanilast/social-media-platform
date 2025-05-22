'use client';
import type { Selection } from '@heroui/react';
import React, { use, useEffect, useState } from 'react';
import { Locale, locales } from '~i18n/i18n.config';
import {
    SelectItem,
    Dropdown,
    DropdownTrigger,
    Button,
    DropdownMenu,
    Badge,
} from '@heroui/react';
import { useLocale, useTranslations } from 'next-intl';
import { setUserLocale } from '~/utils/locale';
import { Languages } from 'lucide-react';

const LangSwitcherTranslationSection = 'LangSwitcher';

export type LangSwitchSelectProps = {
    badgeOnLeft?: boolean;
};

export default function LangSwitchSelect({
    badgeOnLeft = true,
}: LangSwitchSelectProps) {
    const t = useTranslations(LangSwitcherTranslationSection);
    const locale = useLocale();
    const existingLocales = locales;
    const [isOpen, setIsOpen] = useState(false);

    const handleButtonPress = () => {
        setIsOpen(!isOpen);
    };

    const setCurrentLocale = (locale: any) => {
        setIsOpen(false);
        setUserLocale(locale[0] as Locale);
    };

    const [selectedKeys, setSelectedKeys] = useState<Selection>(
        new Set([locale])
    );

    useEffect(() => {
        setCurrentLocale(Array.from(selectedKeys));
    }, [selectedKeys]);

    return (
        <Dropdown isOpen={isOpen}>
            <DropdownTrigger>
                <Badge
                    content={locale.slice(0, 2)}
                    placement={badgeOnLeft ? 'bottom-left' : 'bottom-right'}
                    size="sm"
                    className="share-tech-mono"
                    variant="faded"
                    showOutline={false}
                >
                    <Button
                        className="utility-small-icon min-w-fit"
                        onPress={handleButtonPress}
                    >
                        <Languages />
                    </Button>
                </Badge>
            </DropdownTrigger>
            <DropdownMenu
                disallowEmptySelection
                selectionMode="single"
                selectedKeys={selectedKeys}
                onSelectionChange={setSelectedKeys}
            >
                {existingLocales.map((locale) => (
                    <SelectItem key={locale} onPress={() => setIsOpen(false)}>
                        {t(locale)}
                    </SelectItem>
                ))}
            </DropdownMenu>
        </Dropdown>
    );
}

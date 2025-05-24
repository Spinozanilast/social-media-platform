'use client';
import {
    Button,
    Card,
    CardBody,
    CardHeader,
    Chip,
    cn,
    Divider,
    Dropdown,
    DropdownItem,
    DropdownMenu,
    DropdownTrigger,
} from '@heroui/react';
import Markdown from 'react-markdown';
import { format } from 'date-fns';
import { Roboto } from 'next/font/google';
import remarkGfm from 'remark-gfm';
import remarkBreaks from 'remark-breaks';
import React, { useCallback } from 'react';
import { Calendar, CalendarCog, Edit3Icon, EllipsisVertical, X } from 'lucide-react';
import { Story } from '~api/story/types';
import StoriesService from '~api/story/service';
import { useTheme } from 'next-themes';

type StoryCardProps = {
    story: Story;
    isAuthenticated: boolean;
    onDeleteStoryAction: (storyId: number) => void;
};

const formatStoryDateStamp = (date: Date) => {
    return format(date, 'dd-MM-yyyy HH:mm:ss');
};

const roboto = Roboto({
    subsets: ['latin'],
    weight: ['700'],
});

export default function StoryCard({
    story,
    isAuthenticated,
    onDeleteStoryAction,
}: StoryCardProps) {
    const theme = useTheme();
    const markdownText = story.content.replace(/\\n/gi, '&nbsp;&nbsp;\n');
    const handleDeleteStory = useCallback(async () => {
        try {
            await StoriesService.deleteStory(story.id);
            onDeleteStoryAction(story.id);
        } catch (error) {
            console.error(error);
        }
    }, [onDeleteStoryAction, story.id]);

    return (
        <Card className="w-full">
            <CardHeader className="flex flex-col gap-2">
                <div className="flex flex-row w-full h-min gap-2">
                    <div className="flex flex-row justify-between w-full">
                        <div className="flex flex-row gap-1 items-center">
                            <Calendar className='utility-small-icon' />
                            <p className="story-timestamp">
                                {formatStoryDateStamp(story.createdAt)}
                            </p>
                        </div>
                        {story.updatedAt &&
                            story.updatedAt !== story.createdAt && (
                                <div className="flex flex-row gap-1 items-center">
                                    <CalendarCog className='utility-small-icon' />
                                    <p className="story-timestamp">
                                        {formatStoryDateStamp(story.updatedAt)}
                                    </p>
                                </div>
                            )}
                    </div>
                    {isAuthenticated && (
                        <Dropdown backdrop="opaque">
                            <DropdownTrigger>
                                <button
                                    className="h-fit w-min"
                                >
                                    <EllipsisVertical size={16} />
                                </button>
                            </DropdownTrigger>
                            <DropdownMenu
                                aria-label="Static Actions"
                                variant="faded"
                            >
                                <DropdownItem startContent={<Edit3Icon />} key="edit">
                                    Edit Story
                                </DropdownItem>
                                <DropdownItem
                                    key="delete"
                                    className="text-danger"
                                    color="danger"
                                    startContent={<X />}
                                    onPress={handleDeleteStory}
                                >
                                    Delete Story
                                </DropdownItem>
                            </DropdownMenu>
                        </Dropdown>
                    )}
                </div>
                {story.tags.length > 0 && (
                    <div className="flex flex-row items-center gap-2">
                        {story.tags.map((tag, index) => {
                            if (tag.length > 0) {
                                return (
                                    <Chip color="primary" variant="shadow" key={index}>
                                        {tag}
                                    </Chip>)
                            }

                            return null;
                        })}
                    </div>
                )}
                <h1
                    className={`text-4xl text-accent-orange font-bold ${roboto.className}`}
                >
                    {story.title}
                </h1>
            </CardHeader>
            <Divider />
            <CardBody>
                <Markdown
                    remarkPlugins={[remarkGfm, remarkBreaks]}
                    className="markdown markdown-body"
                >
                    {markdownText}
                </Markdown>
            </CardBody>
        </Card>
    );
};

'use client';
import {
    Button,
    Card,
    CardBody,
    CardHeader,
    Chip,
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
import { EllipsisVertical } from 'lucide-react';
import { Story } from '~api/story/types';
import StoriesService from '~api/story/service';

type StoryCardProps = {
    key: string | number;
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

export const StoryCard: React.FC<StoryCardProps> = ({
    key,
    story,
    isAuthenticated,
    onDeleteStoryAction,
}) => {
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
        <Card className="w-full" key={key}>
            <CardHeader className="flex flex-col gap-2">
                <div className="flex flex-row w-full h-min gap-2">
                    <div className="flex flex-row justify-between w-full">
                        <div className="flex flex-row gap-1">
                            <p className="timestamp-pre-text">Created At</p>
                            <p className="story-timestamp">
                                {formatStoryDateStamp(story.createdAt)}
                            </p>
                        </div>
                        {story.updatedAt &&
                            story.updatedAt !== story.createdAt && (
                                <div className="flex flex-row gap-1">
                                    <p className="timestamp-pre-text">
                                        Edited At
                                    </p>
                                    <p className="story-timestamp">
                                        {formatStoryDateStamp(story.updatedAt)}
                                    </p>
                                </div>
                            )}
                    </div>
                    {isAuthenticated && (
                        <Dropdown backdrop="opaque">
                            <DropdownTrigger>
                                <Button
                                    isIconOnly
                                    className="h-fit w-min"
                                    variant="shadow"
                                >
                                    <EllipsisVertical size={16} />
                                </Button>
                            </DropdownTrigger>
                            <DropdownMenu
                                aria-label="Static Actions"
                                variant="faded"
                            >
                                <DropdownItem key="edit">
                                    Edit Story
                                </DropdownItem>
                                <DropdownItem
                                    key="delete"
                                    className="text-danger"
                                    color="danger"
                                    onPress={handleDeleteStory}
                                >
                                    Delete Story
                                </DropdownItem>
                            </DropdownMenu>
                        </Dropdown>
                    )}
                </div>
                <div className="flex flex-row items-center gap-2">
                    {story.tags.map((tag, index) => (
                        <Chip color="primary" variant="shadow" key={index}>
                            {tag}
                        </Chip>
                    ))}
                </div>
                <h1
                    className={`text-xl text-accent-orange font-bold ${roboto.className}`}
                >
                    {story.title}
                </h1>
            </CardHeader>
            <Divider />
            <CardBody>
                <Markdown
                    remarkPlugins={[remarkGfm, remarkBreaks]}
                    className="markdown"
                >
                    {markdownText}
                </Markdown>
            </CardBody>
        </Card>
    );
};

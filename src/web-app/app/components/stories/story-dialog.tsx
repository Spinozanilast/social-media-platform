'use client';

import { useState, useEffect } from 'react';
import {
    Modal,
    ModalHeader,
    ModalBody,
    ModalContent,
    Input,
    Button,
} from '@heroui/react';
import { useSWRConfig } from 'swr';
import StoriesService from '~api/story/service';
import { CreateStoryModel, Story, UpdateStoryModel } from '~api/story/types';
import { storiesMutationKey } from '~hooks/swr/useStories';
import { storiesCountMutationKey } from '~hooks/swr/useStoriesCount';
import MDEditor from "@uiw/react-md-editor";
import { useTheme } from 'next-themes';
import { useRouter } from 'next/navigation';

type StoryDialogProps = {
    mode: 'create' | 'edit';
    story?: Story;
    isOpen: boolean;
    authorId: string;
    currentPage: number;
    pageSize: number;
    onClose: () => void;
};

export const StoryDialog = ({
    mode = 'create',
    story,
    isOpen,
    authorId,
    currentPage,
    pageSize,
    onClose,
}: StoryDialogProps) => {
    const theme = useTheme();
    const { mutate } = useSWRConfig();
    const [title, setTitle] = useState('');
    const [content, setContent] = useState('');
    const [tags, setTags] = useState('');
    const [isSubmitting, setIsSubmitting] = useState(false);

    useEffect(() => {
        if (mode === 'edit' && story) {
            setTitle(story.title);
            setContent(story.content);
            setTags(story.tags.join(', '));
        }
    }, [mode, story]);

    const handleSubmit = async () => {
        setIsSubmitting(true);

        const payload = {
            title,
            content,
            tags: tags.split(',').map(tag => tag.trim()),
            authorId,
            isShared: story?.isShared || false,
        };

        try {
            if (mode === 'create') {
                await StoriesService.createStory(payload);
            } else if (story?.id) {
                await StoriesService.updateStory(story.id, payload as UpdateStoryModel);
            }

            await mutate(
                storiesMutationKey({
                    authorId,
                    pageNumber: currentPage,
                    pageSize,
                })
            );
            await mutate(storiesCountMutationKey(authorId));
            handleClose();
        } catch (error) {
            console.error(`${mode === 'create' ? 'Creation' : 'Update'} failed:`, error);
        } finally {
            setIsSubmitting(false);
        }
    };

    const handleClose = () => {
        setTitle('');
        setContent('');
        setTags('');
        onClose();
    };

    return (
        <Modal
            isOpen={isOpen}
            onOpenChange={handleClose}
            className="min-w-[75%] min-h-[75%]"
            backdrop="opaque"
        >
            <ModalContent className="h-[90%] overflow-y-auto">
                <ModalHeader className="flex justify-between items-center">
                    <h2 className="text-xl font-semibold">
                        {mode === 'create' ? 'Create New Story' : 'Edit Story'}
                    </h2>
                </ModalHeader>

                <ModalBody className="flex flex-col gap-4">
                    <Input
                        label="Title"
                        placeholder="Enter story title"
                        value={title}
                        onChange={(e) => setTitle(e.target.value)}
                        isRequired
                        isDisabled={isSubmitting}
                    />

                    <div className="flex flex-col gap-1 h-[100%]">
                        <label className="text-sm font-medium">Content:</label>
                        <MDEditor
                            data-color-mode={theme.theme === 'dark' ? 'dark' : 'light'}
                            className="min-h-[95%]"
                            value={content}
                            onChange={(text => setContent(text ?? ""))}
                        />
                    </div>

                    <Input
                        label="Tags"
                        placeholder="Comma-separated tags"
                        value={tags}
                        onChange={(e) => setTags(e.target.value)}
                        isDisabled={isSubmitting}
                    />

                    <Button
                        color="primary"
                        onPress={handleSubmit}
                        isDisabled={!title || !content || isSubmitting}
                        isLoading={isSubmitting}
                    >
                        {isSubmitting
                            ? `${mode === 'create' ? 'Publishing...' : 'Updating...'}`
                            : `${mode === 'create' ? 'Publish Story' : 'Update Story'}`}
                    </Button>
                </ModalBody>
            </ModalContent>
        </Modal>
    );
};

export default function CreateStoryModal(props: Omit<StoryDialogProps, 'mode' | 'story'>) {
    return <StoryDialog mode="create" {...props} />;
}
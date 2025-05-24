'use client';

import { useState } from 'react';
import {
    Modal,
    ModalHeader,
    ModalBody,
    ModalContent,
    Input,
    Button,
} from '@heroui/react';
import MarkdownIt from 'markdown-it';
import { useSWRConfig } from 'swr';
import StoriesService from '~api/story/service';
import { CreateStoryModel } from '~api/story/types';
import { storiesMutationKey } from '~hooks/swr/useStories';
import { storiesCountMutationKey } from '~hooks/swr/useStoriesCount';
import MDEditor, { selectWord } from "@uiw/react-md-editor";


const mdParser = new MarkdownIt();

type CreateStoryModalProps = {
    isOpen: boolean;
    onCloseAction: () => void;
    authorId: string;
    currentPage: number;
    pageSize: number;
};

export const CreateStoryModal = ({
    isOpen,
    onCloseAction,
    authorId,
    currentPage,
    pageSize,
}: CreateStoryModalProps) => {
    const { mutate } = useSWRConfig();
    const [title, setTitle] = useState('');
    const [content, setContent] = useState('');
    const [tags, setTags] = useState('');
    const [isSubmitting, setIsSubmitting] = useState(false);

    const handleSubmit = async () => {
        setIsSubmitting(true);
        const payload: CreateStoryModel = {
            title,
            content,
            tags: tags.split(',').map((tag) => tag.trim()),
            authorId,
            isShared: false,
        };

        try {
            await StoriesService.createStory(payload);

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
            console.error('Story creation failed:', error);
        } finally {
            setIsSubmitting(false);
        }
    };

    const handleClose = () => {
        setTitle('');
        setContent('');
        setTags('');
        onCloseAction();
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
                    <h2 className="text-xl font-semibold">Create New Story</h2>
                </ModalHeader>

                <ModalBody className="flex flex-col gap-4">
                    <Input
                        label="Title"
                        placeholder="Enter story title"
                        value={title}
                        onChange={(value) => setTitle(value.target.value)}
                        isRequired
                        isDisabled={isSubmitting}
                    />

                    <div className="flex flex-col gap-1 h-[100%]">
                        <label className="text-sm font-medium">Content:</label>
                        <MDEditor
                            value={content}
                            className="min-h-full"
                            onChange={(text) => setContent(text ?? "")}
                        />
                    </div>

                    <Input
                        label="Tags"
                        placeholder="Comma-separated tags"
                        value={tags}
                        onChange={() => setTags}
                        isDisabled={isSubmitting}
                    />

                    <Button
                        color="primary"
                        onPress={handleSubmit}
                        isDisabled={!title || !content || isSubmitting}
                        isLoading={isSubmitting}
                    >
                        {isSubmitting ? 'Publishing...' : 'Publish Story'}
                    </Button>
                </ModalBody>
            </ModalContent>
        </Modal>
    );
};

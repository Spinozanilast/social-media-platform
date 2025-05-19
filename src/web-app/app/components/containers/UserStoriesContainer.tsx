'use client';

import { useState } from 'react';
import {
    Button,
    Card,
    CardBody,
    CardHeader,
    Divider,
    Pagination,
    Skeleton,
} from '@heroui/react';
import StoriesService from '@api/story/service';
import { useDisclosure } from '@heroui/react';
import { MessageSquarePlus } from 'lucide-react';
import { StoryCard } from '@components/common/Story';
import useStoriesCount, {
    storiesCountMutationKey,
} from '@hooks/swr/useStoriesCount';
import useStories, { storiesMutationKey } from '@hooks/swr/useStories';
import { CreateStoryModal } from '@components/modals/CreatyStory';
import { Story } from '@api/story/types';
import { mutate } from 'swr';

type UserStoriesContainerProps = {
    userId: string;
    initialStories: Story[];
    isOwner: boolean;
};

const UserStoriesContainer = ({
    userId,
    initialStories,
    isOwner,
}: UserStoriesContainerProps) => {
    const { isOpen, onOpen, onClose } = useDisclosure();
    const [currentPage, setCurrentPage] = useState(1);
    const [pageSize] = useState(10);

    const { stories: storiesData } = useStories(
        { authorId: userId, pageNumber: currentPage, pageSize },
        initialStories
    );

    const { storiesCount } = useStoriesCount(userId);
    const storiesPagesNumber = Math.ceil((storiesCount ?? 0) / pageSize);

    const handlePageChange = (page: number) => {
        setCurrentPage(page);
    };

    const handleDeleteStory = async (storyId: number) => {
        const success = await StoriesService.deleteStory(storyId);
        if (success) {
            await mutate(
                storiesMutationKey({
                    authorId: userId,
                    pageNumber: currentPage,
                    pageSize: pageSize,
                })
            );
            await mutate(storiesCountMutationKey(userId));
        }
    };

    if (!storiesData) {
        return (
            <Card>
                <CardHeader className="flex flex-col gap-2 rounded-md">
                    <Skeleton className="h-5 w-full" />
                    <Skeleton className="h-20 self-center w-40 rounded-md" />
                </CardHeader>
                <Divider />
                <CardBody>
                    <Skeleton className="h-40 rounded-md" />
                </CardBody>
            </Card>
        );
    }

    return (
        <div className="flex flex-col gap-5 items-center justify-center">
            {isOwner && (
                <Button
                    variant="ghost"
                    color="primary"
                    className="w-full p-1"
                    endContent={<MessageSquarePlus />}
                    onPress={onOpen}
                >
                    Create Story
                </Button>
            )}

            <CreateStoryModal
                isOpen={isOpen}
                onCloseAction={onClose}
                authorId={userId}
                currentPage={currentPage}
                pageSize={pageSize}
            />

            {storiesData.map((story) => (
                <StoryCard
                    key={story.id}
                    story={story}
                    isAuthenticated={isOwner}
                    onDeleteStoryAction={handleDeleteStory}
                />
            ))}

            {storiesPagesNumber > 1 && (
                <div className="flex flex-col gap-5 items-center w-full">
                    <Pagination
                        color="primary"
                        variant="faded"
                        page={currentPage}
                        total={storiesPagesNumber}
                        onChange={handlePageChange}
                    />

                    <div className="flex gap-2 justify-center w-full">
                        <Button
                            size="sm"
                            variant="flat"
                            isDisabled={currentPage === 1}
                            onPress={() => handlePageChange(currentPage - 1)}
                        >
                            Previous
                        </Button>
                        <Button
                            color="primary"
                            size="sm"
                            variant="flat"
                            isDisabled={currentPage >= storiesPagesNumber}
                            onPress={() => handlePageChange(currentPage + 1)}
                        >
                            Next
                        </Button>
                    </div>
                </div>
            )}
        </div>
    );
};

export default UserStoriesContainer;

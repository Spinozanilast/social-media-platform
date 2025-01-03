'use client';
import Story from '@models/Stories/stories';
import StoriesService from '@api/services/story';
import { StoryCard } from '@components/common/Story';
import { useCallback, useEffect, useState } from 'react';
import IdentityService from '@/app/api/services/user';
import {
    Button,
    Card,
    CardBody,
    CardHeader,
    Divider,
    Pagination,
    Skeleton,
    Modal,
    useModal,
    ModalHeader,
    ModalBody,
    useDisclosure,
    ModalContent,
} from '@nextui-org/react';
import { BiMessageSquareAdd } from 'react-icons/bi';
import { IoCloseCircleSharp } from 'react-icons/io5';
import CreateStory from '../forms/CreatyStory';

type UserStoriesContainerProps = {
    userId: string;
    userName: string;
};

export const UserStoriesContainer: React.FC<UserStoriesContainerProps> = ({
    userId,
    userName,
}) => {
    const [currentPage, setCurrentPage] = useState(1);

    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [stories, setStories] = useState<Story[]>([]);
    const [storiesCount, setStoriesCount] = useState<number>(0);
    const [storiesPagesNumber, setStoriesPagesNumber] = useState<number>(0);

    const { isOpen, onOpen, onOpenChange } = useDisclosure();

    const fetchIsAuthenticated = useCallback(async () => {
        const isUserAuthenticated = await IdentityService.checkUserIdentity(
            userName
        );
        setIsAuthenticated(isUserAuthenticated);
    }, [userName]);

    const fetchStories = useCallback(async () => {
        const stories = await StoriesService.getAllStories(
            undefined,
            userId,
            currentPage
        );
        setStories(stories);
    }, [userId, currentPage]);

    const fetchStoriesCount = useCallback(async () => {
        const storiesCount = await StoriesService.getAllStoriesCount(
            undefined,
            userId
        );
        setStoriesCount(storiesCount);
    }, [userId]);

    useEffect(() => {
        fetchStoriesCount();
        setStoriesPagesNumber(Math.ceil(storiesCount / 5));
        fetchIsAuthenticated();
        fetchStories();
    }, [fetchIsAuthenticated, fetchStories, fetchStoriesCount, storiesCount]);

    const handleDeleteStory = useCallback(
        (id: number) => {
            setStories((prevStories) =>
                prevStories.filter((story) => story.id !== id)
            );
            fetchStoriesCount();
        },
        [fetchStoriesCount]
    );

    if (stories.length === 0 && storiesCount > 0) {
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
        <div className="flex flex-col gap-5 items-center">
            {isAuthenticated && (
                <Button
                    variant="ghost"
                    color="primary"
                    className="w-full p-1"
                    startContent={<BiMessageSquareAdd className="h-full" />}
                    onPress={onOpen} // Open the modal
                >
                    Create Story
                </Button>
            )}
            <Modal
                className="min-w-[75%] min-h-[75%] overflow-y-auto"
                backdrop="opaque"
                classNames={{
                    backdrop:
                        'bg-gradient-to-t from-zinc-900 to-zinc-900/10 backdrop-opacity-20',
                }}
                isOpen={isOpen}
                onOpenChange={onOpenChange}
            >
                <ModalContent>
                    {(onClose) => (
                        <>
                            <ModalHeader className="h-min">
                                <Button
                                    size="sm"
                                    variant="flat"
                                    color="warning"
                                    onClick={onClose}
                                >
                                    Close
                                </Button>
                            </ModalHeader>
                            <ModalBody className="min-h-full w-full">
                                <CreateStory
                                    onClose={onClose}
                                    authorId={userId}
                                />
                            </ModalBody>
                        </>
                    )}
                </ModalContent>
            </Modal>
            {stories.map((story) => (
                <StoryCard
                    key={story.id}
                    story={story}
                    isAuthenticated={isAuthenticated}
                    onDeleteStory={handleDeleteStory}
                />
            ))}
            <div className="flex flex-col gap-5 items-center">
                <Pagination
                    color="primary"
                    variant="faded"
                    page={currentPage}
                    total={storiesPagesNumber}
                    onChange={setCurrentPage}
                />
                <div className="flex flex-row gap-2 justify-center w-full">
                    {storiesPagesNumber > 1 && (
                        <>
                            <Button
                                size="sm"
                                variant="flat"
                                onPress={() =>
                                    setCurrentPage((prev) =>
                                        prev > 1 ? prev - 1 : prev
                                    )
                                }
                            >
                                Previous
                            </Button>
                            <Button
                                color="primary"
                                size="sm"
                                variant="flat"
                                onPress={() =>
                                    setCurrentPage((prev) =>
                                        prev < storiesPagesNumber
                                            ? prev + 1
                                            : prev
                                    )
                                }
                            >
                                Next
                            </Button>
                        </>
                    )}
                </div>
            </div>
        </div>
    );
};

export default UserStoriesContainer;

import { Story } from '@api/story/types';
import StoriesService from '@api/story/service';
import useSWR from 'swr';

const storiesCountFetcher = async ([, authorId]: [
    string,
    string,
]): Promise<number> => {
    return StoriesService.getAllStoriesCount({ authorId: authorId });
};

export default function useStoriesCount(authorId: string) {
    const { data, isLoading, error } = useSWR<number>(
        storiesCountMutationKey(authorId),
        storiesCountFetcher
    );

    return {
        storiesCount: data,
        isLoading,
        isError: error,
    };
}

export const storiesCountMutationKey = (
    authorId: string
): ['stories-count', string] => ['stories-count', authorId];

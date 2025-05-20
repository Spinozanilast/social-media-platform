import useSWR from 'swr';
import StoriesService from '~api/story/service';
import { Story } from '~api/story/types';

type useStoriesParams = {
    authorId: string;
    pageNumber: number;
    pageSize: number;
};

const storiesFetcher = async ([, authorId, pageNumber, pageSize]: [
    string,
    string,
    number,
    number,
]): Promise<Story[]> => {
    return StoriesService.getAllStories({
        authorId: authorId,
        pageNumber: pageNumber,
        pageSize: pageSize,
    });
};

export default function useStories(
    { authorId, pageNumber, pageSize }: useStoriesParams,
    initialStories?: Story[]
) {
    const { data, isLoading, error } = useSWR<Story[]>(
        storiesMutationKey({
            authorId: authorId,
            pageSize: pageSize,
            pageNumber: pageNumber,
        }),
        storiesFetcher,
        {
            fallbackData: pageNumber === 1 ? initialStories : [],
            revalidateOnMount: true,
        }
    );

    return {
        stories: data,
        isLoading,
        isError: error,
    };
}

export const storiesMutationKey = ({
    authorId,
    pageSize,
    pageNumber,
}: useStoriesParams): ['stories', string, number, number] => [
    'stories',
    authorId,
    pageNumber,
    pageSize,
];

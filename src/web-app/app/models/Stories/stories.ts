export default interface Story {
    id: number;
    authorId: string;
    title: string;
    content: string;
    tags: string[];
    createdAt: Date;
    updatedAt: Date;
    originalPostId?: number | null;
    isShared: boolean;
    isEdited: boolean;
}

export interface CreateStoryModel {
    authorId: string;
    title: string;
    content: string;
    tags: string[];
    isShared: boolean;
}

export interface UpdateStoryModel {
    title: string;
    content: string;
    tags: string[];
    isShared: boolean;
    isEdited: boolean;
    updatedAt?: Date;
    originalPostId?: number | null;
}

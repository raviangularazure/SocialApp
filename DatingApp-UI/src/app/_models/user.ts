import { Photo } from './photo';

export interface User { // mimics the properties in UsersForListDto.cs and UsersForDetailsDto.cs(marked optional at the end)
    id: number;
    username: string;
    knownAs: string;
    age: number;
    gender: string;
    created: Date;
    lastActive: Date;
    photoUrl: string;
    city: string;
    country: string;
    interests?: string;
    lookingFor?: string;
    photos?: Photo[];
    introduction?: string;
}

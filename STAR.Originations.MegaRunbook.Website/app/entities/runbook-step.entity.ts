
import { PBI } from './pbi.entity';
import { Developer } from './developer.entity';
import { Lookup } from './lookup.entity';

export class RunbookStep {
    ID: number;
    GroupNumber: number;
    StepNumber: number;
    Duration: number;

    Team: string;
    Description: string;
    Notes: string;
    Resource: string;
    FormattedTime: string;

    Time: string;

    StatusLookup: Lookup;
    PBI: PBI[];
    Developers: Developer[];

    IsNotCollapsed: boolean;
}

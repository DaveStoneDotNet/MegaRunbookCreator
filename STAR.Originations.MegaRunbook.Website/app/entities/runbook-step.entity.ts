
import { RunbookStepPbi } from './runbook-step-pbi.entity';
import { Contact }        from './contact.entity';
import { Team }           from './team.entity';

export class RunbookStep {
    ID: number;
    GroupNumber: number;
    StepNumber: number;
    Duration: number;

    Name: string;
    Team: string;
    Description: string;
    Notes: string;
    FormattedTime: string;
    RunbookStepStatusCode: string;
    RunbookStepTypeCode: string;

    Time: Date;

    RunbookStepPbis: RunbookStepPbi[];
    Developers: Contact[];
    Resources: Contact[];

    Teams: string[];

    IsHtml: boolean;
    IsNotCollapsed: boolean;
}

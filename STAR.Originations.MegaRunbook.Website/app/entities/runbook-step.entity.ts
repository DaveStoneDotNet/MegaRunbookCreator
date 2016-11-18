
import { RunbookStepPbi }  from './runbook-step-pbi.entity';
import { Contact }         from './contact.entity';
import { Team }            from './team.entity';
import { RunbookTemplate } from './runbook-template.entity';

export class RunbookStep {
    Id: number;
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
    Teams: Team[];
    Developers: Contact[];
    Resources: Contact[];

    IsHtml: boolean;
    IsNotCollapsed: boolean;
}


import { RunbookStep } from './runbook-step.entity';

export class RunbookTemplate {
    Id: number;
    Number: number;
    Name: string;
    RunbookSteps: RunbookStep[];
}

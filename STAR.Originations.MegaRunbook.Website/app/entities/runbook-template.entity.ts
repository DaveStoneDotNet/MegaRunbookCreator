
import { RunbookStep } from './runbook-step.entity';

export class RunbookTemplate {
    ID: number;
    Number: number;
    Name: string;
    RunbookSteps: RunbookStep[];
}

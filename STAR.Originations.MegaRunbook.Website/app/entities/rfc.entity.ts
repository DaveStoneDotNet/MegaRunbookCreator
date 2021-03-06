﻿
import { Contact }         from './contact.entity';
import { Team }            from './team.entity';
import { RunbookStep }     from './runbook-step.entity';
import { RunbookTemplate } from './runbook-template.entity';
import { Paging }          from './paging.entity';

export class RFC {
    Id: number;
    Number: number;
    Name: string;
    Contact: Contact;
    RunbookSteps: RunbookStep[];
    Templates: RunbookTemplate[];
    StartTimeText: string;
    EndTimeText: string;
    StartTime: Date;
    EndTime: Date;

    Paging: Paging;

    constructor() {
        this.Paging = new Paging();
    }
}

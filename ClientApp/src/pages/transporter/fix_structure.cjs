const fs = require('fs');
const path = require('path');

const targetPath = path.join('c:', 'navneet_code_folder', 'navgatix', 'navgatix', 'ClientApp', 'src', 'pages', 'transporter', 'TransporterDashboard.tsx');

let content = fs.readFileSync(targetPath, 'utf8');

// 1. Ensure type is correct
content = content.replace(
    /const \[activeTab, setActiveTab\] = useState<'overview' \| 'drivers' \| 'vehicles'.*?>\('overview'\);/,
    "const [activeTab, setActiveTab] = useState<'overview' | 'drivers' | 'vehicles' | 'requests' | 'reports'>('overview');"
);

// 2. Ensure imports are correct
if (!content.includes("import TransporterReports")) {
    content = content.replace(
        "import TransporterRideRequests from '../../components/TransporterRideRequests';",
        "import TransporterRideRequests from '../../components/TransporterRideRequests';\nimport TransporterReports from '../../components/TransporterReports';"
    );
}

// 3. Fix the middle conditional visibility
content = content.replace(
    "{activeTab !== 'drivers' && (",
    "{(activeTab === 'overview' || activeTab === 'vehicles') && ("
);

// 4. Fix the orphan div / closing brackets around line 585-595
// We want to make sure the fleet table condition closes cleanly.
const tableEndPattern = /<\/table>\s*<\/div>\s*<\/div>\s*<\/div>\s*}\)/;
if (tableEndPattern.test(content)) {
    content = content.replace(tableEndPattern, "</table>\n                        </div>\n                    </div>\n                    )}");
}

// 5. Ensure Reports button is wired
content = content.replace(
    `<button className="w-full flex items-center gap-3 px-4 py-3 rounded-xl text-slate-500 hover:bg-slate-50 hover:text-slate-900 transition-all duration-200">
                            <FileText className="h-5 w-5" />
                            Reports
                        </button>`,
    `<button
                            onClick={() => setActiveTab('reports')}
                            className={\`w-full flex items-center gap-3 px-4 py-3 rounded-xl transition-all duration-200 \${activeTab === 'reports' ? 'bg-primary-50 text-primary-700 font-semibold shadow-sm' : 'text-slate-500 hover:bg-slate-50 hover:text-slate-900'}\`}
                        >
                            <FileText className={\`h-5 w-5 \${activeTab === 'reports' ? 'text-primary-600' : ''}\`} />
                            Reports
                        </button>`
);

// 6. Fix the end of the return block to ensure root div closure.
// The file should end with:
/*
            <VehicleModal ... />
        </div>
    );
};
export default TransporterDashboard;
*/

fs.writeFileSync(targetPath, content, 'utf8');
console.log('done structural fix');

const fs = require('fs');
const path = require('path');

const targetPath = path.join('c:', 'navneet_code_folder', 'navgatix', 'navgatix', 'ClientApp', 'src', 'pages', 'transporter', 'TransporterDashboard.tsx');

let content = fs.readFileSync(targetPath, 'utf8');

// Fix 1: Add TransporterReports import
if (!content.includes("import TransporterReports")) {
    content = content.replace(
        "import TransporterRideRequests from '../../components/TransporterRideRequests';",
        "import TransporterRideRequests from '../../components/TransporterRideRequests';\nimport TransporterReports from '../../components/TransporterReports';"
    );
}

// Fix 2: Update activeTab type
content = content.replace(
    "const [activeTab, setActiveTab] = useState<'overview' | 'drivers' | 'vehicles'>('overview');",
    "const [activeTab, setActiveTab] = useState<'overview' | 'drivers' | 'vehicles' | 'requests' | 'reports'>('overview');"
);

// Fix 3: Update Reports sidebar button
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

// Fix 4: Add Reports tab panel
if (!content.includes("activeTab === 'reports'")) {
    const endOfRequestsTab = content.indexOf("activeTab === 'requests'");
    const nextClosingBracket = content.indexOf(")}", endOfRequestsTab + 50);
    const insertionPoint = nextClosingBracket + 2;
    
    const reportsTabPanel = `
                    {activeTab === 'reports' && (
                        <div className="premium-card overflow-hidden">
                            <div className="p-6 bg-white border-b border-slate-100 mb-4">
                                <h2 className="text-lg font-bold text-slate-900">Analytics & Reports</h2>
                                <p className="text-sm text-slate-500 mt-1">Real-time performance metrics and fleet activity reports.</p>
                            </div>
                            <div className="px-6 pb-6">
                                <TransporterReports userId={currentUser?.userId || currentUser?.UserId} />
                            </div>
                        </div>
                    )}`;
    
    content = content.slice(0, insertionPoint) + reportsTabPanel + content.slice(insertionPoint);
}

fs.writeFileSync(targetPath, content, 'utf8');
console.log('done integration');

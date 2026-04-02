const fs = require('fs');
const path = require('path');

const targetPath = path.join('c:', 'navneet_code_folder', 'navgatix', 'navgatix', 'ClientApp', 'src', 'pages', 'transporter', 'TransporterDashboard.tsx');

let content = fs.readFileSync(targetPath, 'utf8');

// Target the navigation block
const navStart = content.indexOf('<nav className="space-y-2">');
const navEnd = content.indexOf('</nav>', navStart);
if (navStart !== -1 && navEnd !== -1) {
    const newNav = `<nav className="space-y-2">
                        <button
                            onClick={() => setActiveTab('overview')}
                            className={\`w-full flex items-center gap-3 px-4 py-3 rounded-xl transition-all duration-200 \${activeTab === 'overview' ? 'bg-primary-50 text-primary-700 font-semibold shadow-sm' : 'text-slate-500 hover:bg-slate-50 hover:text-slate-900'}\`}
                        >
                            <LayoutDashboard className={\`h-5 w-5 \${activeTab === 'overview' ? 'text-primary-600' : ''}\`} />
                            Overview
                        </button>
                        <button
                            onClick={() => setActiveTab('drivers')}
                            className={\`w-full flex items-center gap-3 px-4 py-3 rounded-xl transition-all duration-200 \${activeTab === 'drivers' ? 'bg-primary-50 text-primary-700 font-semibold shadow-sm' : 'text-slate-500 hover:bg-slate-50 hover:text-slate-900'}\`}
                        >
                            <Users className={\`h-5 w-5 \${activeTab === 'drivers' ? 'text-primary-600' : ''}\`} />
                            Manage Drivers
                        </button>
                        <button
                            onClick={() => setActiveTab('vehicles')}
                            className={\`w-full flex items-center gap-3 px-4 py-3 rounded-xl transition-all duration-200 \${activeTab === 'vehicles' ? 'bg-primary-50 text-primary-700 font-semibold shadow-sm' : 'text-slate-500 hover:bg-slate-50 hover:text-slate-900'}\`}
                        >
                            <Truck className={\`h-5 w-5 \${activeTab === 'vehicles' ? 'text-primary-600' : ''}\`} />
                            Vehicle Fleet
                        </button>
                        <button
                            onClick={() => setActiveTab('requests')}
                            className={\`w-full flex items-center gap-3 px-4 py-3 rounded-xl transition-all duration-200 \${activeTab === 'requests' ? 'bg-primary-50 text-primary-700 font-semibold shadow-sm' : 'text-slate-500 hover:bg-slate-50 hover:text-slate-900'}\`}
                        >
                            <Package className={\`h-5 w-5 \${activeTab === 'requests' ? 'text-primary-600' : ''}\`} />
                            Ride Requests
                        </button>
                        <button
                            onClick={() => setActiveTab('reports')}
                            className={\`w-full flex items-center gap-3 px-4 py-3 rounded-xl transition-all duration-200 \${activeTab === 'reports' ? 'bg-primary-50 text-primary-700 font-semibold shadow-sm' : 'text-slate-500 hover:bg-slate-50 hover:text-slate-900'}\`}
                        >
                            <FileText className={\`h-5 w-5 \${activeTab === 'reports' ? 'text-primary-600' : ''}\`} />
                            Reports
                        </button>
                    </nav>`;
    
    content = content.slice(0, navStart) + newNav + content.slice(navEnd + 6);
}

fs.writeFileSync(targetPath, content, 'utf8');
console.log('done final sidebar fix');
